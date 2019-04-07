#pragma once
#include "Temp_hum_sensor.h"

/**
*	Pass parameter inputs to parent constructor arguments.
*/
Temp_hum_sensor::Temp_hum_sensor(const String& name, const int& pin) : Sensor(name, pin)
{	
	Sensor::data_list.add("temperature");
	Sensor::data_list.add("humidity");
}

/**
*	Read all data the sensor can provide, and return as a Result pointer.
*/
const Sensor::sub_list& Temp_hum_sensor::read_sensor()
{
	DHT.read11(pin);

	data::Subscribable* val = result.get("temperature");
	data::result_double d_res = (data::result_double)val;
	data::val_of(d_res) = DHT.humidity;

	
	val = result.get("humidity");
	d_res = (data::result_double)val;
	data::val_of(d_res) = DHT.humidity;

	return result;
}
