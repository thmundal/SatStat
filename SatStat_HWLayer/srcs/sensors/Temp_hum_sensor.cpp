#pragma once
#include "Temp_hum_sensor.h"

/**
*	Pass parameter inputs to parent constructor arguments.
*/
Temp_hum_sensor::Temp_hum_sensor(const String& name, const int& pin) : Sensor(name, pin)
{		
	sstl::Lists::add_entry<double>("temperature");
	sstl::Lists::add_entry<double>("humidity");	
}

/**
*	Read all data the sensor can provide, and return as a Result pointer.
*/
void Temp_hum_sensor::read_sensor()
{
	DHT.read11(pin);

	auto val = sstl::Lists::get_data("temperature");
	auto d_res = sstl::downcast<double>(val);
	d_res->set_data(DHT.temperature);
	
	val = sstl::Lists::get_data("humidity");
	d_res = sstl::downcast<double>(val);
	d_res->set_data(DHT.humidity);
}
