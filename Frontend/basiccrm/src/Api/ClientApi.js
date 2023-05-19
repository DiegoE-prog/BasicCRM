import axios from "axios"

const getClientsAsync = async () => {
	const response = await axios.get("https://localhost:7081/api/Client/")
	return response
}

const getClientAsync = async (id) => {
	const response = await axios.get(`https://localhost:7081/api/Client/${id}`)
	return response
}

const addClientAsync = async (client) => {
	const response = await axios.post("https://localhost:7081/api/Client/", client)
	return response
}

const editClientAsync = async (client) => {
	const response = await axios.put("https://localhost:7081/api/Client/", client)
	return response
}

const deleteClientAsync = async (id) => {
	const response = await axios.delete(`https://localhost:7081/api/Client/${id}`)
	return response
}

export { getClientAsync, getClientsAsync, addClientAsync, editClientAsync, deleteClientAsync }
