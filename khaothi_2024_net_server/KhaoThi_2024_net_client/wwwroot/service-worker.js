//// service-worker.js
//self.addEventListener('install', event => {
//    self.skipWaiting();
//});

//self.addEventListener('activate', event => {
//    event.waitUntil(clients.claim());
//});

//self.addEventListener('fetch', event => {
//    event.respondWith(
//        fetch(event.request)
//            .catch(error => {
//                return caches.match(event.request);
//            })
//    );
//});


// service-worker.js
const CACHE_NAME = 'khaothi-2024-cache-v1';

// Tài nguyên cần cache trước khi sử dụng (app shell)
const PRECACHE_ASSETS = [
    '/',
    '/index.html',
    '/css/app.css',
    '/css/blazored-modal.css',
    '/css/BankSelect.css',
    '/favicon.png',
    '/manifest.json',
    '/js/security.js',
    '/_content/MudBlazor/MudBlazor.min.css',
    '/_content/MudBlazor/MudBlazor.min.js',
    '/_content/Radzen.Blazor/css/default.css',
    '/_content/Radzen.Blazor/Radzen.Blazor.js'
];

// Blazor WebAssembly resources to cache on first use
const BLAZOR_ASSETS = [
    '/_framework/'
];

// Sự kiện cài đặt service worker
self.addEventListener('install', event => {
    //console.log('[Service Worker] Installing...');
    // Bước 1: Pre-cache app shell
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => {
                // console.log('[Service Worker] Pre-caching app shell');
                return cache.addAll(PRECACHE_ASSETS);
            })
            .then(() => {
                // console.log('[Service Worker] Pre-caching complete');
                return self.skipWaiting();
            })
            .catch(error => {
                // console.error('[Service Worker] Pre-caching failed:', error);
            })
    );
});

// Sự kiện kích hoạt (sau khi install hoặc update)
self.addEventListener('activate', event => {
    //console.log('[Service Worker] Activating...');

    // Xóa cache cũ
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames.filter(name => {
                    return name !== CACHE_NAME;
                }).map(name => {
                    // console.log('[Service Worker] Deleting old cache:', name);
                    return caches.delete(name);
                })
            );
        })
            .then(() => {
                //console.log('[Service Worker] Claiming clients');
                return self.clients.claim();
            })
    );
});

// Xử lý fetch requests
self.addEventListener('fetch', event => {
    const url = new URL(event.request.url);

    // Không cache các API requests
    if (url.pathname.startsWith('/api/')) {
        return;
    }

    // Không cache các requests có query params (thường là dynamic content)
    if (url.search && !url.pathname.startsWith('/_framework/')) {
        return;
    }

    // Cache-first strategy cho Blazor WebAssembly resources
    if (BLAZOR_ASSETS.some(asset => url.pathname.startsWith(asset))) {
        event.respondWith(cacheFirst(event.request));
        return;
    }

    // Network-first strategy cho các static assets đã pre-cache
    if (PRECACHE_ASSETS.includes(url.pathname) ||
        url.pathname.endsWith('.css') ||
        url.pathname.endsWith('.js') ||
        url.pathname.endsWith('.png') ||
        url.pathname.endsWith('.jpg') ||
        url.pathname.endsWith('.svg')) {
        event.respondWith(networkFirst(event.request));
        return;
    }

    // Cho các request khác, thử network trước và fallback vào cache
    event.respondWith(networkFirst(event.request));
});

// Cache-first strategy: ưu tiên lấy từ cache, nếu không có thì lấy từ network và cập nhật cache
async function cacheFirst(request) {
    const cachedResponse = await caches.match(request);
    if (cachedResponse) {
        return cachedResponse;
    }

    try {
        const networkResponse = await fetch(request);
        if (networkResponse && networkResponse.status === 200) {
            const cache = await caches.open(CACHE_NAME);
            cache.put(request, networkResponse.clone());
        }
        return networkResponse;
    } catch (error) {
        // console.error('[Service Worker] Cache-first fetch failed:', error);
        // Nếu không có cache và network fail, trả về response lỗi
        return new Response('Network request failed', { status: 408, headers: { 'Content-Type': 'text/plain' } });
    }
}

// Network-first strategy: ưu tiên lấy từ network, nếu fail thì lấy từ cache
async function networkFirst(request) {
    try {
        // Thử lấy từ network
        const networkResponse = await fetch(request);

        // Nếu thành công, cache lại response (chỉ khi status 200 OK)
        if (networkResponse && networkResponse.status === 200) {
            const cache = await caches.open(CACHE_NAME);
            cache.put(request, networkResponse.clone());
        }

        return networkResponse;
    } catch (error) {
        //console.log('[Service Worker] Network request failed, falling back to cache for', request.url);

        // Nếu network fail, thử lấy từ cache
        const cachedResponse = await caches.match(request);
        if (cachedResponse) {
            return cachedResponse;
        }

        // Nếu không có trong cache, trả về response lỗi
        // console.error('[Service Worker] No cache available for', request.url);
        return new Response('Network request failed and no cache available', {
            status: 503,
            headers: { 'Content-Type': 'text/plain' }
        });
    }
}

// Xử lý thông báo push
self.addEventListener('push', event => {
    if (event.data) {
        const data = event.data.json();

        const options = {
            body: data.body || 'Có thông báo mới',
            icon: '/favicon.png',
            badge: '/favicon.png',
            data: {
                url: data.url || '/'
            }
        };

        event.waitUntil(
            self.registration.showNotification(data.title || 'Thông báo KhaoThi', options)
        );
    }
});

// Xử lý khi người dùng click vào notification
self.addEventListener('notificationclick', event => {
    event.notification.close();

    if (event.notification.data && event.notification.data.url) {
        event.waitUntil(
            clients.openWindow(event.notification.data.url)
        );
    }
});