import React from "react"
import { NavLink } from "react-router-dom"
import DataTable from "react-data-table-component"
import { useState, useEffect } from "react"
import { getAdressesAsync } from "../../Api/AddressApi"

function AddressIndex() {
	const [addresses, setAddresses] = useState([])
	const [loading, setLoading] = useState(false)

	const columns = [
		{
			name: "Address Line",
			selector: (row) => row.addressLine
		},
		{
			name: "Address Details",
			selector: (row) => row.addressDetails
		},
		{
			name: "City",
			selector: (row) => row.city
		},
		{
			name: "State",
			selector: (row) => row.state
		},
		{
			name: "Zip Code",
			selector: (row) => row.zipCode
		},
		{
			name: "Contry",
			selector: (row) => row.country
		},
		{
			selector: (row) => (
				<div>
					<NavLink to={`/Address/EditAddress/${row.addressID}`} className="btn btn-success">
						<i class="bi bi-pencil-square"></i>
					</NavLink>

					<button className="btn btn-danger">
						<i class="bi bi-trash"></i>
					</button>
				</div>
			)
		}
	]

	async function fetchTableData() {
		setLoading(true)

		const responseFromApi = await getAdressesAsync()
		const addresses = await responseFromApi.data

		setAddresses(addresses)
		setLoading(false)
	}

	useEffect(() => {
		fetchTableData()
	}, [])

	return (
		<React.Fragment>
			<div className="row pb-2 pt-2">
				<div className="col">
					<span className="h1">Address List</span>
				</div>
				<div class="col text-end pt-1">
					<NavLink to="/Address/AddAddress" className="btn btn-outline-primary">
						<i class="fas fa-plus"></i> Add New Address
					</NavLink>
				</div>
			</div>
			<div className="row pb-1 pt-2">
				<DataTable columns={columns} data={addresses} progressPending={loading} pagination />
			</div>
		</React.Fragment>
	)
}

export default AddressIndex
