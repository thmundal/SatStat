#pragma once
#include "Sensor.h"
#include "../../lib/HX711_ADC/HX711_ADC.h"

class Load_cell : public Sensor
{
public:
	Load_cell(const String& name);
	~Load_cell();

	void read_sensor() override;

private:
	HX711_ADC* m_load_cell;

	const int m_dt_pin = 6;
	const int m_sck_pin = 7;

	const float m_length = 0.077; // Meter
	float m_value; // Nm
};