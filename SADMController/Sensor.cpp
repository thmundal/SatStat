#pragma once
#include "Sensor.h"

Sensor::Sensor(String name, int pin)
{
	this->name = name;
	this->pin = pin;
}

String Sensor::get_name()
{
	return this->name;
}
