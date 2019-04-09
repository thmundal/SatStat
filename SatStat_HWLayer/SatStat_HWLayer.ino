#pragma once
#include "./srcs/HWLayer.h"

HWLayer hw;

void setup()
{
	Serial.begin(9600);
	hw.setup();
}

void loop()
{	
	hw.loop();
}