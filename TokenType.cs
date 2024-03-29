﻿namespace TinyCompiler;

public enum TokenType {
    Eof = -1,
    Newline,
    Number,
    Ident,
    String,
    Label = 101,
    Goto,
    Print,
    Input,
    Let,
    If,
    Then,
    Endif,
    While,
    Repeat,
    EndWhile,
    Eq = 201,
    Plus,
    Minus,
    Asterisk,
    Slash,
    EqEq,
    NotEq,
    Lt,
    LtEq,
    Gt,
    GtEq
}