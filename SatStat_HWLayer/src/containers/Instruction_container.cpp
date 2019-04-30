#include "Instruction_container.h"

Instruction_container::Instruction_container()
{	
	SADM_functions::init_stepper();

	m_available_instructions.append("auto_rotate",
		new Instruction
		(
			"auto_rotate",
			parse_params
			(				
				Uf_param("enable", "bool")
			),
			SADM_functions::set_auto_rotate
		)
	);

	m_available_instructions.append("rotate_steps",
		new Instruction
		(
			"rotate_steps",
			parse_params
			(				
				Uf_param("steps", "int")
			),
			SADM_functions::rotate
		)
	);

	m_available_instructions.append("rotate_deg", 
		new Instruction
		(
			"rotate_deg",
			parse_params
			(				
				Uf_param("deg", "float")
			),
			SADM_functions::rotate
		)
	);	

	m_available_instructions.append("subscribe",
		new Instruction
		(
			"subscribe",
			parse_params
			(				
				Uf_param("data_list", "JsonArray")
			),
			Request_functions::subscribe
		)
	);

	m_available_instructions.append("unsubscribe",
		new Instruction
		(
			"unsubscribe",
			parse_params
			(
				Uf_param("data_list", "JsonArray")
			),
			Request_functions::unsubscribe
		)
	);

	m_available_instructions.append("reset",
		new Instruction
		(
			"reset",
			parse_params
			(
				Uf_param("none", "none")
			),
			Request_functions::reset
		)
	);
}

Instruction* Instruction_container::get(const String & ins)
{
	return m_available_instructions.get(ins);
}

LinkedList<String, Instruction*>& Instruction_container::get_available_instructions()
{	
	return m_available_instructions;
}