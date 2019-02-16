#pragma once
#include "Arduino.h"
#include "Sensor.h"
#include "../libraries/DHT.h"

class Temperature_sensor : public Sensor
{
public:
	Temperature_sensor(String name, int pin);
	int read_sensor();
private:
	DHT* dht;
};