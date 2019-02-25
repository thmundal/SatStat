#include "Json_object_container.h"

Json_object_container::Json_object_container() : Json_container()
{	
	Json_container::json = &Json_container::buffer->createObject();
}

void Json_object_container::create()
{	
	delete Json_container::buffer;
	Json_container::buffer = new DynamicJsonBuffer;

	Json_container::json = &Json_container::buffer->createObject();
}

void Json_object_container::parse(const String &json)
{
	delete Json_container::buffer;
	Json_container::buffer = new DynamicJsonBuffer;

	Json_container::json = &Json_container::buffer->parseObject(json);
}

JsonObject* Json_object_container::get()
{
	return Json_container::json;
}
