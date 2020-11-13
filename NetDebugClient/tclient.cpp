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

	addr.sin_family		= AF_INET;
	addr.sin_port = htons(serverport);
	addr.sin_addr.S_un.S_addr = inet_addr(serverip);

	BOOL bDontLinger	= FALSE;
	setsockopt(Socket, SOL_SOCKET, SO_DONTLINGER,(const char*)&bDontLinger, sizeof(BOOL));

	int nNetTimeout = 1000;
	setsockopt(Socket, SOL_SOCKET, SO_SNDTIMEO,	(char*)&nNetTimeout, sizeof(int));
	setsockopt(Socket, SOL_SOCKET, SO_RCVTIMEO,	(char*)&nNetTimeout, sizeof(int));

	pBuf = (char*)malloc(m_sockMaxBufSize);

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
	if (Socket != NULL)
		closesocket(Socket);

	Socket = NULL;

	while (cmdQ.empty() == false)
	{
		MyStructData* pStruct = cmdQ.front();
		cmdQ.pop();

		if (pStruct == NULL || pStruct->pdata == NULL || Socket == NULL)
			continue;

		free(pStruct->pdata);
		pStruct->pdata = NULL;

		free(pStruct);
		pStruct = NULL;
	}

	if (pBuf)
	{
		free(pBuf);
		pBuf = NULL;
	}

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


void tclient::Send()
{
	if (cmdQ.empty())
		return;

	MyStructData* pStruct = cmdQ.front();
	cmdQ.pop();

	if (pStruct == NULL || pStruct->pdata == NULL || Socket == NULL)
		return;

	::send(Socket, pStruct->pdata, pStruct->len, 0);

	free(pStruct->pdata);
	pStruct->pdata	= NULL;

	free(pStruct);
	pStruct			= NULL;
}

void tclient::Recv()
{
	::memset(pBuf,0,this->m_sockMaxBufSize);
	//int n = ::recv(Socket, pBuf, this->m_sockMaxBufSize,0);
	//if (n > 0)
	//{
	//	std::cout << pBuf << std::endl;
	//}
	int n = ::recv(Socket, pBuf, this->m_sockMaxBufSize, 0);
	if (n > 0)
	{
		std::cout << pBuf << std::endl;
	}
}

void tclient::PushCmd(const char* cmd, const int len)
{
	MyStructData* pStruct	= new MyStructData;
	pStruct->len			= len;
	pStruct->pdata			= (char*)malloc(pStruct->len);

	if (pStruct->pdata != NULL)
	{
		::memset(pStruct->pdata, 0, pStruct->len);
		::memmove(pStruct->pdata, cmd, len);
		cmdQ.push(pStruct);
	}
}



