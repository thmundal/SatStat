#pragma once
#include "Arduino.h"
#include "Sensor.h"
#include "../libraries/dht.h"

class Temp_hum_sensor : public Sensor
{
public:
	Temp_hum_sensor(const String& name, const int& pin);	
	const Result* read_sensor();	
private:	
	dht DHT;
};