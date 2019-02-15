#pragma once
#include "Arduino.h"
#include "DS1302.h"

class Sensor
{
public:
	Sensor(String name, int pin);
	String read_sensor();
private:
	String name;
	int pin;
	DS1302* clock;
};
