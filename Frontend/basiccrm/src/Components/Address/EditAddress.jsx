import React from "react"
import { useParams } from "react-router-dom"
import { useEffect, useState } from "react"
import { getAddressAsync } from "../../Api/AddressApi"

function EditAddress() {
	const [address, setAddress] = useState([])
	const { id } = useParams()

	useEffect(() => {
		const fetchAddress = async () => {
			const responseFromApi = await getAddressAsync(id)
			const address = responseFromApi.data
			setAddress(address)
		}
		fetchAddress()
	}, [])

	const handleEdit = () => {
		console.log(address)
	}

	return (
		<div className="row p-2">
			<div className="col-12 h4">Edit Address</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Address Line" value={address.addressLine} onChange={(e) => setAddress({ ...address, addressLine: e.target.value })} />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Address Details" value={address.addressDetails} onChange={(e) => setAddress({ ...address, addressDetails: e.target.value })} />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="City" value={address.city} onChange={(e) => setAddress({ ...address, city: e.target.value })} />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="State" value={address.state} onChange={(e) => setAddress({ ...address, state: e.target.value })} />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Zip Code" value={address.zipCode} onChange={(e) => setAddress({ ...address, zipCode: e.target.value })} />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Country" value={address.country} onChange={(e) => setAddress({ ...address, country: e.target.value })} />
			</div>
			<div className="col-12 col-md-6 offset-md-3 p-2">
				<button className="btn btn-primary btn-sm form-control" onClick={handleEdit}>
					Edit
				</button>
			</div>
		</div>
	)
}

export default EditAddress
