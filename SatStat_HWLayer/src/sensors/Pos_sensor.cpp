#include "Pos_sensor.h"

// Definition of static members
const int Pos_sensor::m_pin = A0;
int Pos_sensor::m_val;
int Pos_sensor::m_last_val;
float Pos_sensor::m_pos;

/**
*	Constructor.
*	Configuring sensor pin and adds the sensor as entry in the sstl data list.
*/
Pos_sensor::Pos_sensor(const String& name) : Sensor(name)
{
	pinMode(m_pin, INPUT);
	sstl::Lists::add_entry<float>("position");
}

/**
*	Sets the current position as zero reference.
*/
void Pos_sensor::tare()
{
	m_pos = 0;
	m_val = analogRead(m_pin);
	m_last_val = m_val;
}

/**
*	Interprets the incoming analog value from the sensor and updates the value of the data list entry.
*/
void Pos_sensor::read_sensor()
{
	m_val = analogRead(m_pin);

	if (m_val != m_last_val)
	{
		if (m_val > m_last_val - 50 && m_val < m_last_val + 50)
		{
			if (m_val > m_last_val)
			{
				m_pos += ((m_val - m_last_val) / m_val_inc_per_deg) / SADM_functions::m_ratio;
			}
			else
			{
				m_pos -= ((m_last_val - m_val) / m_val_inc_per_deg) / SADM_functions::m_ratio;
			}
		}
		else
		{
			if (m_val > m_last_val)
			{
				m_pos -= ((m_hv - m_val + m_last_val - m_lv) / m_val_inc_per_deg) / SADM_functions::m_ratio;
			}
			else
			{
				m_pos += ((m_hv - m_last_val + m_val - m_lv) / m_val_inc_per_deg) / SADM_functions::m_ratio;
			}
		}
		m_last_val = m_val;
	}

	sstl::Lists::set_data("position", m_pos);
}
