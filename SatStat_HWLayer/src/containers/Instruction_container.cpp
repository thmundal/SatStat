#include "Instruction_container.h"

/**
*	Constructor appending every available instruction to the list.
*/
Instruction_container::Instruction_container()
{	
	m_available_instructions.append("set_step_size",
		new Instruction
		(
			"set_step_size",
			parse_params
			(
				Uf_param("step_size", "float")
			),
			SADM_functions::set_step_size
		)
	);
	m_available_instructions.append("set_stepping_mode",
		new Instruction
		(
			"set_stepping_mode",
			parse_params
			(
				Uf_param("divisor", "int")
			),
			SADM_functions::set_stepping_mode
		)
	);

	m_available_instructions.append("set_gear_ratio",
		new Instruction
		(
			"set_gear_ratio",
			parse_params
			(
				Uf_param("ratio", "float")
			),
			SADM_functions::set_ratio
		)
	);

	m_available_instructions.append("set_speed",
		new Instruction
		(
			"set_speed",
			parse_params
			(
				Uf_param("rpm", "float")
			),
			SADM_functions::set_speed
		)
	);

	m_available_instructions.append("set_direction",
		new Instruction
		(
			"set_direction",
			parse_params
			(
				Uf_param("direction", "string")
			),
			SADM_functions::set_dir
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

	m_available_instructions.append("rotate_degrees", 
		new Instruction
		(
			"rotate_degrees",
			parse_params
			(				
				Uf_param("degrees", "float")
			),
			SADM_functions::rotate
		)
	);
}

/**
*	Returns a spesific instruction in the list.
*/
Instruction* Instruction_container::get(const String & ins)
{
	return m_available_instructions.get(ins);
}

/**
*	Returns the entire list of available instructions.
*/
LinkedList<String, Instruction*>& Instruction_container::get_available_instructions()
{	
	return m_available_instructions;
}