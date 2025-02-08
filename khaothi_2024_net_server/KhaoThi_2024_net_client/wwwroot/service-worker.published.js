// service-worker.published.js
// Tên và version của cache
const CACHE_NAME = 'khaothi-cache-v1';
const DATA_CACHE_NAME = 'khaothi-data-cache-v1';

// Danh sách các tài nguyên cần cache
const RESOURCES_TO_CACHE = [
    '/',
    '/index.html',
    '/_framework/blazor.webassembly.js',
    '/_framework/blazor.boot.json',
    '/manifest.json',
    '/icon-192.png',
    '/icon-512.png',
    '/css/app.css',
    '/css/mudblazor.min.css'
];

// Sự kiện install - cache các tài nguyên tĩnh
self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME).then(cache => {
            console.log('[ServiceWorker] Pre-caching offline resources');
            return cache.addAll(RESOURCES_TO_CACHE);
        })
    );
});

// Sự kiện activate - xóa cache cũ
self.addEventListener('activate', event => {
    event.waitUntil(
        caches.keys().then(cacheNames => {
            return Promise.all(
                cacheNames
                    .filter(cacheName => {
                        return cacheName.startsWith('khaothi-') &&
                            cacheName !== CACHE_NAME &&
                            cacheName !== DATA_CACHE_NAME;
                    })
                    .map(cacheName => {
                        console.log('[ServiceWorker] Removing old cache', cacheName);
                        return caches.delete(cacheName);
                    })
            );
        })
    );
});

// Sự kiện fetch - xử lý cache và network requests
self.addEventListener('fetch', event => {
    if (event.request.method !== 'GET') return;

    // API calls
    if (event.request.url.includes('/api/')) {
        event.respondWith(
            caches.open(DATA_CACHE_NAME).then(async cache => {
                try {
                    const response = await fetch(event.request);
                    if (response.ok) {
                        cache.put(event.request, response.clone());
                    }
                    return response;
                } catch (err) {
                    const cachedResponse = await cache.match(event.request);
                    return cachedResponse || new Response('No connection to the server', {
                        status: 504,
                        statusText: 'Gateway Timeout'
                    });
                }
            })
        );
        return;
    }

    // Static resources
    event.respondWith(
        caches.match(event.request).then(response => {
            if (response) {
                return response;
            }

            return fetch(event.request).then(response => {
                if (!response || response.status !== 200 || response.type !== 'basic') {
                    return response;
                }

                const responseToCache = response.clone();
                caches.open(CACHE_NAME).then(cache => {
                    cache.put(event.request, responseToCache);
                });

                return response;
            });
        })
    );
});

// Xử lý push notifications
self.addEventListener('push', event => {
    const options = {
        body: event.data.text(),
        icon: '/icon-192.png',
        badge: '/icon-192.png'
    };

    event.waitUntil(
        self.registration.showNotification('KhaoThi', options)
    );
});

// Xử lý click vào notification
self.addEventListener('notificationclick', event => {
    event.notification.close();
    event.waitUntil(
        clients.openWindow('/')
    );
});