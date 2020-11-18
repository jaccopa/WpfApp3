#include <event2/event.h>
#include <event2/bufferevent.h>
#include <string.h>


void event_cb(struct bufferevent* bev, short events, void* ptr)
{
    if (events & BEV_EVENT_CONNECTED) {
        /* We're connected to 127.0.0.1:8080.   Ordinarily we'd do
           something here, like start reading or writing. */
        puts("BEV_EVENT_CONNECTED");
    }
    else if (events & BEV_EVENT_ERROR) {
        /* An error occured while connecting. */
        puts("BEV_EVENT_ERROR");
    }
}


int main5(int argc, char** argv)
{
#ifdef _WIN32
    WSADATA wsa_data;
    WSAStartup(0x0201, &wsa_data);
#endif

    struct event_base* base;
    struct bufferevent* bev;
    struct sockaddr_in sin;

    base = event_base_new();

    memset(&sin, 0, sizeof(sin));
    sin.sin_family = AF_INET;
    sin.sin_addr.s_addr = htonl(0xc0a86f6f); /* 127.0.0.1 */
    sin.sin_port = htons(2000); /* Port 8080 */

    bev = bufferevent_socket_new(base, -1, BEV_OPT_CLOSE_ON_FREE);

    bufferevent_setcb(bev, NULL, NULL, event_cb, NULL);

    if (bufferevent_socket_connect(bev,
        (struct sockaddr*)&sin, sizeof(sin)) < 0) {
        /* Error starting connection */
        bufferevent_free(bev);
        return -1;
    }

    event_base_dispatch(base);
    return 0;
}