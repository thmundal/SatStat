#pragma once
#include "Arduino.h"
#include "Sensor.h"
#include "dht.h"

class Temp_sensor : public Sensor
{
public:
	Temp_sensor(String name, int pin) : Sensor(name, pin) {};
	int read_sensor();
private:
	dht DHT;
};