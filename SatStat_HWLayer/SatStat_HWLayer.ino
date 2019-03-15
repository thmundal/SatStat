#pragma once
#include "./srcs/HWLayer.h"

HWLayer satstat_hwlayer;

void setup()
{
	satstat_hwlayer.setup();	
}

void loop()
{	
	satstat_hwlayer.loop();
}