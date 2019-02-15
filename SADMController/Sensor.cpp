#pragma once
#include "Sensor.h"

Sensor::Sensor(String name, int pin)
{
	this->name = name;
	this->pin = pin;

	// ce_pin, io_pin, sclk_pin
	clock = new DS1302(1, 2, 3);
	//Time test(2019, 2, 14, 14, 59, 0, Time::kThursday);
	//clock->time(test);
}

String Sensor::read_sensor()
{
	String tmp = String((clock->time()).day)
		+ String((clock->time()).mon)
		+ String((clock->time()).yr)
		+ String((clock->time()).hr)
		+ String((clock->time()).min)
		+ String((clock->time()).sec);

	return "{\"sensor\":\"" + name 
		+ "\",\"time\":" + tmp 
		+ ",\"data\":" + String(digitalRead(pin)) + "}";
}
