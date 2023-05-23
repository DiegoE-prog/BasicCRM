import React, { Fragment } from "react"
import { NavLink } from "react-router-dom"
import DataTable from "react-data-table-component"
import { useState, useEffect } from "react"
import { deleteClientAsync, getClientsAsync } from "../../Api/ClientApi"
import Swal from "sweetalert2"
import withReactContent from "sweetalert2-react-content"

function ClientIndex() {
	const [clients, setClients] = useState([])
	const [loading, setLoading] = useState(false)
	const [deleteTrigger, setDeleteTrigger] = useState(false)
	const MySwal = withReactContent(Swal)

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
			selector: (row) => row.dateOfBirthday
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

					<button className="btn btn-danger" onClick={() => handleDelete(row.clientID)}>
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
		setClients(clients)
		setLoading(false)
	}

	useEffect(() => {
		fetchTableData()
	}, [deleteTrigger])

	const handleDelete = (id) => {
		MySwal.fire({
			title: "Are you sure",
			text: "You won't be able to revert this!",
			icon: "warning",
			showCancelButton: true,
			confirmButtonColor: "#3085d6",
			cancelButtonColor: "#d33",
			confirmButtonText: "Yes, delete it!"
		}).then((result) => {
			if (result.isConfirmed) {
				const response = deleteClientAsync(id)
				response.then((response) => {
					if (response.data.success) {
						MySwal.fire("Deleted!", "The Client has been deleted.", "success")
						setDeleteTrigger(!deleteTrigger)
					}
				})
			}
		})
	}

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
