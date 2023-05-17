import React from "react"
import { useEffect, useState } from "react"
import { getAdressesAsync } from "../../Api/AddressApi"
import Select from "react-select"

function AddClient() {
	const [addressOptions, setAddressOptions] = useState([])
	const [loading, setLoading] = useState([])

	useEffect(() => {
		async function fetchSelectData() {
			setLoading(true)
			const response = await getAdressesAsync()
			const addresses = await response.data
			const addressOptions = addresses.map((address) => {
				return { value: address.addressID, label: address.addressLine }
			})
			setAddressOptions(addressOptions)
			setLoading(false)
		}
		fetchSelectData()
	}, [])

	return (
		<div className="row p-2">
			<div className="col-12 h4">Add New Client</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="First Name" />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Last Name" />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input type="date" className="form-control form-control-sm" placeholder="Date of Birth" />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Email" />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Phone Number" />
			</div>
			<div className="col-12 col-md-4 p-1">
				<Select options={addressOptions} isClearable={true} isLoading={loading} placeholder="Select an Address" />
			</div>
			<div className="col-12 col-md-6 offset-md-3 p-2">
				<button className="btn btn-primary btn-sm form-control">Add</button>
			</div>
		</div>
	)
}

export default AddClient
