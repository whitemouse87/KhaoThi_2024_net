/* Manifest version: GnOuI73u */
// service-worker.js
self.addEventListener('install', event => {
    self.skipWaiting();
});

self.addEventListener('activate', event => {
    event.waitUntil(clients.claim());
});

self.addEventListener('fetch', event => {
    event.respondWith(
        fetch(event.request)
            .catch(error => {
                return caches.match(event.request);
            })
    );
});