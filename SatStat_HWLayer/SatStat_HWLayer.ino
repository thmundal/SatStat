#pragma once
#include "./srcs/SatStat_HWLayer.h"

SatStat_HWLayer satstat_hwlayer;

void setup()
{
	satstat_hwlayer.setup();
}

void loop()
{	
	satstat_hwlayer.loop();
}