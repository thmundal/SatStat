#pragma once

template <class KEY, class VALUE>
class Node
{
public:
	Node();
	Node(const KEY & key);
	Node(const KEY & key, const VALUE & val);
	Node(const Node<KEY, VALUE> &other);
	~Node();

	Node<KEY, VALUE> &operator=(const Node<KEY, VALUE> &other);

	void add(const KEY &key);
	void append(const KEY &key, const VALUE &value);
	VALUE & get_index(int & index, VALUE & default_value);
	VALUE &get(const KEY &key, VALUE &default_value);
	bool set(const KEY &key, const VALUE& value);
	bool erase(const KEY &key, Node*& parent_m_next);
	bool contains(const KEY &key) const;

private:
	Node *m_next;
	KEY m_key;
	VALUE m_value;
};

template<class KEY, class VALUE>
inline Node<KEY, VALUE>::Node()
{
	m_next = 0;
}

template<class KEY, class VALUE>
inline Node<KEY, VALUE>::Node(const KEY& key)
{
	m_next = 0;
	m_key = key;
}

template<class KEY, class VALUE>
inline Node<KEY, VALUE>::Node(const KEY& key, const VALUE& val)
{
	m_next = 0;
	m_key = key;
	m_value = val;
}

template<class KEY, class VALUE>
inline Node<KEY, VALUE>::Node(const Node<KEY, VALUE>& other)
{	
	*this = other;
}

template<class KEY, class VALUE>
inline Node<KEY, VALUE>::~Node()
{
	if (m_next != 0)
	{
		delete m_next;
	}
}

// Jon later som han skjønner
template<class KEY, class VALUE>
inline Node<KEY, VALUE>& Node<KEY, VALUE>::operator=(const Node<KEY, VALUE>& other)
{

	if (m_next != 0)
	{
		delete m_next;
	}

	m_key = other.m_key;
	m_value = other.m_value;

	if (other.m_next != 0)
	{
		m_next = new Node<KEY, VALUE>;
		*m_next = *other.m_next;
	}

	return *this;
}

// Jon tester
template<class KEY, class VALUE>
inline void Node<KEY, VALUE>::add(const KEY &key)
{
	if (m_next == 0)
	{
		m_next = new Node(key);
	}
	else
	{
		m_next->add(key);
	}
}

template<class KEY, class VALUE>
inline void Node<KEY, VALUE>::append(const KEY &key, const VALUE &value)
{
	if (m_next == 0)
	{
		m_next = new Node(key, value);
	}
	else
	{
		m_next->append(key, value);
	}
}

template<class KEY, class VALUE>
inline VALUE &Node<KEY, VALUE>::get_index(int& index, VALUE &default_value)
{
	if (index == 0)
	{
		return m_value;
	}
	else if (m_next != 0)
	{
		return m_next->get_index(--index, default_value);
	}

	return default_value;
}

template<class KEY, class VALUE>
inline VALUE &Node<KEY, VALUE>::get(const KEY &key, VALUE &default_value)
{
	if (m_key == key)
		return m_value;
	if (m_next != 0)
		return m_next->get(key, default_value);
	return default_value;
}

// Jon tester
template<class KEY, class VALUE>
inline bool Node<KEY, VALUE>::set(const KEY &key, const VALUE& value)
{
	if (m_key == key)
	{
		m_value = value;
	}
	else if (m_next != 0)
	{
		return m_next->set(key, value);
	}
	else
	{
		append(key, value);
		return true;
	}
	return false;
}

template<class KEY, class VALUE>
inline bool Node<KEY, VALUE>::erase(const KEY & key, Node *& parent_m_next)
{
	if (m_key == key)
	{
		parent_m_next = m_next;
		m_next = 0;
		delete this;
		return true;
	}
	else if (m_next != 0)
	{
		return m_next->erase(key, m_next);
	}
	return false;
}

template<class KEY, class VALUE>
inline bool Node<KEY, VALUE>::contains(const KEY &key) const
{
	if (m_key == key)
		return true;
	if (m_next != 0)
		return m_next->contains(key);
	return false;
}