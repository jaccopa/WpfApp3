#include "tclient.h"



tclient* pclient = NULL;

void WINAPI onTimeFuncSendCmd(UINT wTimerID, UINT msg, DWORD dwUser, DWORD dwl, DWORD dw2)
{
    printf("onTimeFuncSendCmd00000000000000000\n");

    pclient->Send();

    return;
}

void WINAPI onTimeFuncGenCmd(UINT wTimerID, UINT msg, DWORD dwUser, DWORD dwl, DWORD dw2)
{
    //printf("onTimeFuncGenCmd111111111111111111\n");

    pclient->PushCmd("123", strlen("123"));
    return;
}


void WINAPI RecvFunc(void *para)
{
    while (true)
    {
        pclient->Recv();
        pclient->Send();
        Sleep(1);
    }
}

int main(int argc, char** argv)
{
    MMRESULT timer_idCmd, timer_idCmdGen;
    int n = 0;

    pclient = new tclient("192.168.111.111",2000);

    while (pclient->Connect() == false)
    {
    }


    _beginthread(RecvFunc,0,NULL);

    //timer_idCmd = timeSetEvent(500, 0, (LPTIMECALLBACK)onTimeFuncSendCmd, DWORD(1), TIME_PERIODIC);
    //if (NULL == timer_idCmd)
    //{
    //    printf("timeSetEvent() failed with error %d\n", GetLastError());
    //    return 0;
    //}

    timer_idCmdGen = timeSetEvent(50, 0, (LPTIMECALLBACK)onTimeFuncGenCmd, DWORD(1), TIME_PERIODIC);
    if (NULL == timer_idCmdGen)
    {
        printf("timeSetEvent() failed with error %d\n", GetLastError());
        return 0;
    }

    //timeKillEvent(timer_idCmd);

    while (true)
    {
       int k = getchar();
    }

    return 0;
}





