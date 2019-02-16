#pragma once
#include "Arduino.h"
#include "Sensor.h"
#include "../libraries/DHT.h"

class Humidity_sensor : public Sensor
{
public:
	Humidity_sensor(String name, int pin);
	int read_sensor();
private:
	DHT* dht;
};