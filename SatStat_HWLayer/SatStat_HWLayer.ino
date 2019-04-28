/*
 Name:		Instruction_test_proj.ino
 Created:	25-Apr-19 14:08:10
 Author:	skjel
*/

#include "./src/HWLayer.h"

HWLayer hwlayer;

void setup() 
{	
	hwlayer.setup();
}

void loop() 
{
	hwlayer.loop();
}