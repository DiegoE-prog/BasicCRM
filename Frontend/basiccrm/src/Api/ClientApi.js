import axios from "axios"

const getClientsAsync = async () => {
	const response = await axios.get("https://localhost:7081/api/Client/")
	return response
}

const getClientAsync = async (id) => {
	const response = await axios.get(`https://localhost:7081/api/Client/${id}`)
	return response
}

export { getClientAsync, getClientsAsync }
