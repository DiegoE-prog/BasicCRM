import React, { Fragment } from "react"
import { NavLink } from "react-router-dom"
import DataTable from "react-data-table-component"
import { useState, useEffect } from "react"
import { getClientsAsync } from "../../Api/ClientApi"

function ClientIndex() {
	const [clients, setClients] = useState([])
	const [loading, setLoading] = useState(false)

	const columns = [
		{
			name: "First Name",
			selector: (row) => row.firstName
		},
		{
			name: "Last Name",
			selector: (row) => row.lastName
		},
		{
			name: "Date of Birth",
			selector: (row) => row.dateOfBirth
		},
		{
			name: "Email",
			selector: (row) => row.email
		},
		{
			name: "Phone Number",
			selector: (row) => row.phoneNumber
		},
		{
			name: "Address Line",
			selector: (row) => (row.address != null ? row.address.addressLine : "")
		},
		{
			name: "City",
			selector: (row) => (row.address != null ? row.address.city : "")
		},
		{
			name: "State",
			selector: (row) => (row.address != null ? row.address.state : "")
		},
		{
			selector: (row) => (
				<div>
					<NavLink to={`/Client/EditClient/${row.clientID}`} className="btn btn-success">
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
		const response = await getClientsAsync()
		const clients = response.data.content
		console.log(clients)
		setClients(clients)
		setLoading(false)
	}

	useEffect(() => {
		fetchTableData()
	}, [])

	return (
		<Fragment>
			<div className="row pb-2 pt-2">
				<div className="col">
					<span className="h1">Client List</span>
				</div>
				<div class="col text-end pt-1">
					<NavLink to="/Client/AddClient" className="btn btn-outline-primary">
						<i class="fas fa-plus"></i> Add New Client
					</NavLink>
				</div>
			</div>
			<div className="row pb-1 pt-2">
				<DataTable columns={columns} data={clients} progressPending={loading} pagination />
			</div>
		</Fragment>
	)
}

export default ClientIndex
