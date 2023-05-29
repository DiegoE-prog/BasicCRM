import React from "react"

class DirectoryIndex extends React.Component {
	render() {
		return <span className="h1">Welcome {process.env.NODE_ENV}</span>
	}
}

export default DirectoryIndex
