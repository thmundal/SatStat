#pragma once
#include "Temp_hum_sensor.h"

Temp_hum_sensor::Temp_hum_sensor(String name, int pin) : Sensor(name, pin)
{
}

String Temp_hum_sensor::read_sensor()
{
	DHT.read11(pin);
	return "{\"temperature\":" 
		+ String(DHT.temperature) + ","
		+ "\"humidity\":" 
		+ String(DHT.humidity) + "}";
}
