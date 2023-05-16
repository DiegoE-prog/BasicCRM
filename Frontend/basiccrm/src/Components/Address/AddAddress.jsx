import React from "react"

function AddAddress() {
	return (
		<div className="row p-2">
			<div className="col-12 h4">Add new Address</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Address Line" />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Address Details" />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="City" />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="State" />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Zip Code" />
			</div>
			<div className="col-12 col-md-4 p-1">
				<input className="form-control form-control-sm" placeholder="Country" />
			</div>
			<div className="col-12 col-md-6 offset-md-3 p-2">
				<button className="btn btn-primary btn-sm form-control">Add</button>
			</div>
		</div>
	)
}

export default AddAddress
