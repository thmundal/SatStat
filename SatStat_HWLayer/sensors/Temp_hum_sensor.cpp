#pragma once
#include "Temp_hum_sensor.h"

// Pass parameter inputs to parent constructor arguments
Temp_hum_sensor::Temp_hum_sensor(const String& name, const int& pin) : Sensor(name, pin, 2)
{	
	result[0].name = "temperature";
	result[1].name = "humidity";
}

// Read and return every reading the sensor support
const Result* Temp_hum_sensor::read_sensor()
{
	DHT.read11(pin);
	result[0].data = DHT.temperature;
	result[1].data = DHT.humidity;

	return result;
}
