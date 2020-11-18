/*
  This example program provides a trivial server program that listens for TCP
  connections on port 9995.  When they arrive, it writes a short message to
  each client connection, and closes each connection once it is flushed.

  Where possible, it exits cleanly in response to a SIGINT (ctrl-c).
*/

//#pragma comment(lib,"ws2_32.lib")

#include <string.h>
#include <errno.h>
#include <stdio.h>
#include <signal.h>
#ifndef _WIN32
#include <netinet/in.h>
# ifdef _XOPEN_SOURCE_EXTENDED
#  include <arpa/inet.h>
# endif
#include <sys/socket.h>
#endif

//#include <event2/bufferevent.h>
//#include <event2/buffer.h>
//#include <event2/listener.h>
//#include <event2/util.h>
//#include <event2/event.h>
#include <event2/event.h>
#include <iostream>

struct cb_arg
{
    struct event* ev;
    struct timeval tv;
};

void timeout_cb(intptr_t fd, short event, void* params)
{
    static int k = 0;
    std::cout << k++ << std::endl;

    struct cb_arg* arg = (struct cb_arg*)params;
    struct event* ev = arg->ev;
    struct timeval tv = arg->tv;
    evtimer_add(ev, &tv);
}


int main4(int argc, char** argv)
{
    struct event_base* base = event_base_new();
    struct event* timeout = NULL;
    struct timeval tv = { 0, 30 };
    struct cb_arg arg;

    timeout = evtimer_new(base, timeout_cb, &arg);
    arg.ev = timeout;
    arg.tv = tv;
    evtimer_add(timeout, &tv);
    event_base_dispatch(base);
    evtimer_del(timeout);


    return 0;
}
