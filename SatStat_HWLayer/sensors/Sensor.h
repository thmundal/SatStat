#pragma once
#include "Arduino.h"
#include "Result.h"

class Sensor
{
public:
	Sensor(const String& name, const int& pin, const int& data_count = 1);
	~Sensor();

	virtual const Result* read_sensor() = 0;
	const String& get_name() const;
	const int& get_data_count() const;

protected:
	String name;
	int pin;	
	int data_count;
	Result* result;	
};