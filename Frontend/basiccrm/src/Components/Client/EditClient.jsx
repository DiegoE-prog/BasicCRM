import React from "react"
import { useEffect, useState } from "react"
import { editClientAsync, getClientAsync } from "../../Api/ClientApi"
import { useParams } from "react-router"
import FormClient from "./FormClient"
import { useNavigate } from "react-router-dom"
import Swal from "sweetalert2"
import withReactContent from "sweetalert2-react-content"

function EditClient() {
	const { id } = useParams()
	const [client, setClient] = useState({
		clientID: "",
		firstName: "",
		lastName: "",
		dateOfBirthday: "",
		email: "",
		phoneNumber: "",
		addressID: ""
	})
	const MySwal = withReactContent(Swal)
	const navigate = useNavigate()

	const fetchClient = async () => {
		const response = await getClientAsync(id)
		const client = await response.data.content
		setClient(client)
	}

	useEffect(() => {
		fetchClient()
	}, [])

	const onSubmit = async () => {
		const clientData = {
			clientID: client.clientID,
			firstName: client.firstName,
			lastName: client.lastName,
			dateOfBirth: new Date(client.dateOfBirthday),
			email: client.email,
			phoneNumber: client.phoneNumber,
			addressID: client.addressID
		}
		const response = await editClientAsync(clientData)
		if (response.data.success) {
			MySwal.fire({
				position: "top-end",
				icon: "success",
				title: response.data.message,
				showConfirmButton: false,
				timer: 2000
			})
			navigate("/Client/ClientIndex")
		}
	}

	return (
		<div className="row p-2">
			<div className="col-12 h4">Edit Client</div>
			<FormClient onSubmit={onSubmit} client={client} setClient={setClient} />
		</div>
	)
}

export default EditClient
