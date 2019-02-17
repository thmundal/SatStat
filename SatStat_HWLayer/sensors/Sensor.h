#pragma once
#include "Arduino.h"

class Sensor
{
public:
	Sensor(String name, int pin);
	virtual String read_sensor() = 0;
	String get_name();

protected:
	String name;
	int pin;
};
