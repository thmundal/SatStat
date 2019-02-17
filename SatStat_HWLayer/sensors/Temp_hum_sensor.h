#pragma once
#include "Arduino.h"
#include "Sensor.h"
#include "../libraries/dht.h"

class Temp_hum_sensor : public Sensor
{
public:
	Temp_hum_sensor(String name, int pin);	
	String read_sensor();
private:
	dht DHT;
};