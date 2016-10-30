// 这是主 DLL 文件。

#include "Stdafx.h"

#include "Flexlive.CQP.CQCProxy.h"

using namespace FlexliveCQP;

void CQCPlugin::Initialize(String^ appDirectory)
{
	try
	{
		appDirectory = Path::Combine(appDirectory, "..\\");
		appDirectory = Path::Combine(appDirectory, "..\\");
		appDirectory = Path::Combine(appDirectory, "Flexlive.CQP.Framework.dll");

		array<unsigned char>^ buffer = File::ReadAllBytes(appDirectory);

		if (buffer->Length <= 0)
		{
			throw gcnew Exception("Length 0");
		}

		for each (Type^ T in Assembly::Load(buffer)->GetTypes())
		{
			if (T->Name == "CQNativeClrProxy")
			{
				CQNaviteProxyObject = Activator::CreateInstance(T);
				CQNaviteProxyMethod = T->GetMethod("ReceiveMessage");
				break;
			}
		}
	}
	catch (Exception ^ex)
	{

	}
}

void CQCPlugin::SendMessage(String^ message)
{
	try
	{
		array<Object^>^ params = gcnew array<Object^>(1){ message };
		Object^ returnValue = CQNaviteProxyMethod->Invoke(CQNaviteProxyObject, params);
	}
	catch (Exception^ ex)
	{

	}
}

void CQCPlugin::CSharpTrans(String^ eventType, int subType, int sendTime, long fromGroup, long fromDiscuss,
	long fromQQ, String^ fromAnonymous, long beingOperateQQ, String^ msg, int font, String^ responseFlag, String^ file)
{
	try
	{
		String^ message = String::Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}",
			eventType, subType, sendTime, fromGroup, fromDiscuss, fromQQ, fromAnonymous,
			beingOperateQQ, msg->Replace("|","$内容分割$"), font, responseFlag, file);

		array<Object^>^ params = gcnew array<Object^>(1){ message };
		Object^ returnValue = CQNaviteProxyMethod->Invoke(CQNaviteProxyObject, params);
	}
	catch (Exception^ ex)
	{

	}
}