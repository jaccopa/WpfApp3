#include "tclient.h"


bool tclient::Init(const char* serverip, const int serverport)
{
	WORD wVersionRequested;
	WSADATA wsaData;

	wVersionRequested = MAKEWORD(2, 2);
	if (WSAStartup(wVersionRequested, &wsaData) != 0)
	{
		return false;
	}

	Socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (Socket == INVALID_SOCKET)
	{
		return false;
	}

	addr.sin_family = AF_INET;
	addr.sin_port = htons(serverport);
	addr.sin_addr.S_un.S_addr = inet_addr(serverip);

	return true;
}


bool tclient::Connect()
{
	if (connect(Socket, (sockaddr*)&addr, sizeof(sockaddr_in)) == SOCKET_ERROR)
	{
		return false;
	}

	return true;
}

void tclient::DisConnect()
{
	if (timer_id != NULL)
		timeKillEvent(timer_id);

	if (thSend != NULL)
		CloseHandle(thSend);

	if (thRecv != NULL)
		CloseHandle(thRecv);

	if (Socket != NULL)
		closesocket(Socket);

	WSACleanup();
}

tclient::~tclient()
{
	DisConnect();
}

tclient::tclient(const char* serverip, const int serverport)
{
	Init(serverip, serverport);
}




