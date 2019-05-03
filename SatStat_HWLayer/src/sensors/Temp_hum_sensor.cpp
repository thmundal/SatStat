#pragma once
#include "Temp_hum_sensor.h"

/**
*	Constructor. Passes the parameter inputs to parent constructor arguments,
*	initializes dht and adds temperature and humidity entries to the SSTL data list.
*/
Temp_hum_sensor::Temp_hum_sensor(const String& name, const int& pin) : Sensor(name, pin)
{		
	dht = new DHT_Unified(pin, DHT22);
	dht->begin();

	sstl::Lists::add_entry<float>("temperature");
	sstl::Lists::add_entry<float>("humidity");
}

/**
*	Destructor. Deletes dht.
*/
Temp_hum_sensor::~Temp_hum_sensor()
{
	delete dht;
}

/**
*	Read all data the sensor can provide and update the SSTL list with the new readings.
*/
void Temp_hum_sensor::read_sensor()
{		
	sensors_event_t event;

	dht->temperature().getEvent(&event);

	if (!isnan(event.temperature))
	{
		sstl::Lists::set_data("temperature", event.temperature);
	}

	dht->humidity().getEvent(&event);

	if (!isnan(event.relative_humidity))
	{
		sstl::Lists::set_data("humidity", event.relative_humidity);
	}
}
