import axios from "axios"

const URL = process.env.NODE_ENV === "development" ? "https://localhost:7081" : "dev-crmbasic-wepapi.azurewebsites.net"

const getClientsAsync = async () => {
	const response = await axios.get(`${URL}/api/Client/`)
	return response
}

const getClientAsync = async (id) => {
	const response = await axios.get(`${URL}/api/Client/${id}`)
	return response
}

const addClientAsync = async (client) => {
	const response = await axios.post(`${URL}/api/Client/`, client)
	return response
}

const editClientAsync = async (client) => {
	const response = await axios.put(`${URL}/api/Client/`, client)
	return response
}

const deleteClientAsync = async (id) => {
	const response = await axios.delete(`${URL}/api/Client/${id}`)
	return response
}

export { getClientAsync, getClientsAsync, addClientAsync, editClientAsync, deleteClientAsync }
