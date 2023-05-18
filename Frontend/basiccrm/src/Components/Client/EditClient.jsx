import React from "react"
import { useEffect, useState } from "react"
import { getAdressesAsync } from "../../Api/AddressApi"
import { getClientAsync } from "../../Api/ClientApi"
import Select from "react-select"
import { useParams } from "react-router"

function EditClient() {
	const [addressOptions, setAddressOptions] = useState([])
	const [client, setClient] = useState([])
	const [loading, setLoading] = useState([])
	const [select, setSelect] = useState([])
	const { id } = useParams()

	async function fetchSelectData() {
		setLoading(true)
		const response = await getAdressesAsync()
		const addresses = await response.data.content
		const addressOptions = addresses.map((address) => {
			return { value: address.addressID, label: address.addressLine }
		})
		setAddressOptions(addressOptions)
		setLoading(false)
	}

	async function fetchClient() {
		const response = await getClientAsync(id)
		const client = await response.data.content
		setClient(client)
		setSelect(handleValue(client))
	}

	const handleValue = (client) => {
		return client.address != null ? { label: client.address.addressLine, value: client.address.addressID } : ""
	}

	useEffect(() => {
		fetchSelectData()
		fetchClient()
	}, [])

	const handleClick = () => {
		console.log(client)
	}

	return (
		<div className="row p-2">
			<div className="col-12 h4">Add New Client</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="First Name" value={client.firstName} onChange={(e) => setClient({ ...client, firstName: e.target.value })} />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Last Name" value={client.lastName} onChange={(e) => setClient({ ...client, lastName: e.target.value })} />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input type="date" className="form-control form-control-sm" placeholder="Date of Birth" value={client.dateOfBirth} onChange={(e) => setClient({ ...client, dateOfBirth: e.target.value })} />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Email" value={client.email} onChange={(e) => setClient({ ...client, email: e.target.value })} />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Phone Number" value={client.phoneNumber} onChange={(e) => setClient({ ...client, phoneNumber: e.target.value })} />
			</div>
			<div className="col-12 col-md-4 p-1">
				<Select
					options={addressOptions}
					isLoading={loading}
					placeholder="Select an Address"
					value={select}
					onChange={(e) => {
						setSelect({ value: e.value, label: e.label })
						setClient({ ...client, addressID: e.value, address: null })
					}}
				/>
			</div>
			<div className="col-12 col-md-6 offset-md-3 p-2">
				<button className="btn btn-primary btn-sm form-control" onClick={handleClick}>
					Edit
				</button>
			</div>
		</div>
	)
}

export default EditClient
