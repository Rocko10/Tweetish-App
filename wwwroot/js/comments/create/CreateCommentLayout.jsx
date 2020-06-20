import React from 'react'

export default class CreateCommentLayout extends React.Component {

    constructor(props) {
        super(props)

        this.state = {
            text: '',
            tweetId: -1,
            comment: ''
        }

        this.handleClickedComment = this.handleClickedComment.bind(this)
        this.handleOnChangeInputComment = this.handleOnChangeInputComment.bind(this)
        this.selectedComment = this.selectedComment.bind(this)

        window.addEventListener('selectedComment', this.selectedComment, false)
    }

    selectedComment(e) {
        this.setState({
            tweetId: e.detail.tweetId,
            text: e.detail.text
        })
    }

    handleClickedComment(e) {
        if (this.state.tweetId == -1 || this.state.comment == '') {
            return
        }

        const req = {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                tweetId: this.state.tweetId,
                text: this.state.comment
            })
        }

        fetch(`/comments/create/${this.state.tweetId}`, req)
        .then(res => {
            if (res.status != 200) {
                return
            }

            this.setState({
                text: '',
                tweetId: -1,
                comment: ''
            })
        })
    }

    handleOnChangeInputComment(e) {
        this.setState({comment: e.target.value})
    }

    render() {
        return <div>
            <label>Leave a comment</label>
            <p> {this.state.text} </p>
            <textarea value={this.state.comment} onChange={this.handleOnChangeInputComment}></textarea>
            <button onClick={this.handleClickedComment} disabled={this.state.tweetId == -1}>
                Comment
            </button>
        </div>
    }

}