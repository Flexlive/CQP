#include "stdafx.h"
#include "appmain.h"

using namespace slimecs;

void CSDelegate::Init()
{
	//先通过 CQ_getAppDirectory 获取到 应用目录. 然后通过这个应用目录.载入 cs 插件
	//并且运行执行 cs 里面的一个初始化命令 (用来关联 事件 , 主要也是处理这个)
}