import React from "react"
import FormClient from "./FormClient"
import { useState } from "react"
import { addClientAsync } from "../../Api/ClientApi"
import { useNavigate } from "react-router-dom"
import Swal from "sweetalert2"
import withReactContent from "sweetalert2-react-content"

function AddClient() {
	const [client, setClient] = useState({
		firstName: "",
		lastName: "",
		dateOfBirth: "",
		email: "",
		phoneNumber: "",
		addressID: ""
	})

	const MySwal = withReactContent(Swal)
	const navigate = useNavigate()

	const onSubmit = async () => {
		const clientData = {
			firstName: client.firstName,
			lastName: client.lastName,
			dateOfBirth: client.dateOfBirth,
			email: client.email,
			phoneNumber: client.phoneNumber,
			addressID: client.addressID
		}

		const response = await addClientAsync(clientData)
		console.log(response)
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
			<div className="col-12 h4">Add New Client</div>
			<FormClient onSubmit={onSubmit} client={client} setClient={setClient} />
		</div>
	)
}

export default AddClient
