#pragma once
#include <winsock2.h>
#pragma comment(lib , "Ws2_32.lib")
#include <Mmsystem.h>
#pragma comment(lib, "winmm.lib")
#include <Windows.h>
#include <process.h>
#include <thread>
#include "lockFreeQueue.h"

struct MyStructData
{
	int len;
	char* pdata;
};

class tclient
{
public:
	tclient(const char* serverip, const int serverport);
	bool Connect();
	void DisConnect();
	void Send();
	void PushCmd(const char* cmd, const int len);

	~tclient();

private:

	bool Init(const char* serverip, const int serverport);



	void Recv();


	SOCKET		Socket;
	sockaddr_in addr;
	CLockFreeQueue<MyStructData*, 1000>  cmdQ;
};

