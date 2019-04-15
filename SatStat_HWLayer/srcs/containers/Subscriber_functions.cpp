#pragma once
#include "Subscriber_functions.h"

void Subscriber_functions::subscribe(Json_container<JsonObject>* request)
{
	JsonObject& ins_obj = request->get();

	if (ins_obj.containsKey("parameters"))
	{
		if (ins_obj.is<JsonArray>("parameters"))
		{
			JsonArray& params = ins_obj.get<JsonArray>("parameters");
			String str;

			for (size_t i = 0; i < params.measureLength(); i++)
			{
				str = params[i].asString();
				sstl::Lists::subscribe(str);
			}			
		}
		else
		{
			request->create();
			JsonObject& tmp_obj = request->get();
			tmp_obj.set("error", "Invalid parameters!");
			tmp_obj.printTo(Serial);
		}
	}
	else
	{
		request->create();
		JsonObject& tmp_obj = request->get();
		tmp_obj.set("error", "Invalid argument!");
		tmp_obj.printTo(Serial);
	}
}

void Subscriber_functions::unsubscribe(Json_container<JsonObject>* request)
{
	JsonObject& ins_obj = request->get();

	if (ins_obj.containsKey("parameters"))
	{
		if (ins_obj.is<JsonArray>("parameters"))
		{
			JsonArray& params = ins_obj.get<JsonArray>("parameters");
			String str;

			for (size_t i = 0; i < params.measureLength(); i++)
			{
				str = params[i].asString();
				sstl::Lists::unsubscribe(str);
			}
		}
		else
		{
			request->create();
			JsonObject& tmp_obj = request->get();
			tmp_obj.set("error", "Invalid parameters!");
			tmp_obj.printTo(Serial);
		}
	}
	else
	{
		request->create();
		JsonObject& tmp_obj = request->get();
		tmp_obj.set("error", "Invalid argument!");
		tmp_obj.printTo(Serial);
	}
}
