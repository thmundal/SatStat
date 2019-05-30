#include "Pos_sensor.h"

Pos_sensor::Pos_sensor(const String& name) : Sensor(name)
{
	pinMode(m_pin, INPUT);
	sstl::Lists::add_entry<float>("position");
	m_pos = 0;
	m_last_val = m_lv + 100;
}

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
				m_pos -= (1 / m_val_inc_per_deg) / SADM_functions::m_ratio;
			}
			else
			{
				m_pos += (1 /m_val_inc_per_deg) / SADM_functions::m_ratio;
			}
		}
		m_last_val = m_val;
	}

	sstl::Lists::set_data("position", m_pos);

	// OLD
	/*m_pos = ((m_val - m_lv) / m_val_inc_per_deg) - 180;

	if (m_pos > 180.0f)
	{
		m_pos = 180;
	}
	else if (m_pos < -180.0f)
	{
		m_pos = -180;
	}*/
}
