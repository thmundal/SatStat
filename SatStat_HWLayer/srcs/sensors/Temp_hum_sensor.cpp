#pragma once
#include "Temp_hum_sensor.h"

/**
*	Pass parameter inputs to parent constructor arguments.
*/
Temp_hum_sensor::Temp_hum_sensor(const String& name, const int& pin) : Sensor(name, pin, 2)
{	
	result[0].set_name("temperature");
	result[1].set_name("humidity");
}

/**
*	Read all data the sensor can provide, and return as a Result pointer.
*/
const Result* Temp_hum_sensor::read_sensor()
{
	DHT.read11(pin);
	result[0].set_data(DHT.temperature);
	result[1].set_data(DHT.humidity);

	return result;
}
