
vs2019 ����libevent�����в��Դ��� 

������https://libevent.org/�������µ�Stable releases libevent-2.1.11-stable.tar.gz
����ǰ��׼��
��ѹ��֮���޸�����5���ļ�
������ event_iocp.c evthread_win32.c listener.c �м���һ���궨�� #define _WIN32_WINNT 0x0500
�����޸� minheap-internal.h ������һ��UINT32_MAX�ĺ궨�岻��֧�� �ĳ�UINT_MAX
�����޸� Makefile.nmake �ҵ� CFLAGS=$(CFLAGS) /Ox /W3 /wd4996 /nologo /Zi ��ؼ������һ��ѡ������
��ʼ����
�� x86 Native Tools Command Prompt for VS 2019 �л��� libevent-2.1.11-stable Ŀ¼��ִ�� nmake /f Makefile.nmake
������3��lib�ļ� 
����libevent_core.lib��All core event and buffer functionality. This library contains all the event_base, evbuffer, bufferevent, and utility functions.
����libevent_extras.lib��This library defines protocol-specific functionality that you may or may not want for your application, including HTTP, DNS, and RPC.
����libevent.lib��This library exists for historical reasons; it contains the contents of both libevent_core and libevent_extra. You shouldn��t use it; it may go away in a future version of Libevent.
������ʹ�� libevent.lib
������ɺ�ʹ��
��Ŀ¼����������һ�� WIN32-Code ��WIN32-Code\nmakeĿ¼�µ��������ݸ��Ƶ� include�� ���ɿ�ʼʹ��
�ڲ��Թ�������� include ����Ŀ¼�����������������ws2_32.lib;wsock32.lib;libevent_core.lib
���Խ��