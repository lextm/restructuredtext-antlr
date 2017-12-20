grammar ReStructuredText;

// Copyright (C) 2011 Bart Kiers
// Copyright (C) 2017 Lex Li
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

options {
  language=CSharp;
}

parse
  :  (element | empty_line) + EOF
  ;

element
  :  section | sectionElement
  ;
  
sectionElement
  :  comment | listItemBullet | paragraph | listItemEnumerated | lineBlock
  ;

comment
  :  Space* Comment (line+)?
  ;

paragraph
  :  line+
  ;
  
section
  :  (Section LineBreak)? title Section (LineBreak)+ sectionElement*
  ;
  
lineBlock
  :  (Block line)+
  ;  

line
  :  indentation? lineStart lineEnd LineBreak
  ;
  
lineStart
  :  span
  |  textStart
  ;

lineEnd
  :  lineAtom*
  ;

lineAtom
  :  span | textEnd
  ;

empty_line
  :  Space* LineBreak
  ;

indentation
  :  Space+
  ;

textStart
  : text_fragment_start text_fragment*
  ;

textEnd
  : textAtoms
  ;
  
textAtoms
  : text_fragment+
  ;
  
title
  : text_fragment+ LineBreak
  ;
 
span
  :  starText
  |  reference
  |  referenceIn
  |  hyperlinkTarget
  |  hyperlink
  |  hyperlinkDoc
  |  backTickText
  ;
 
text_fragment_start
  :  Section
  |  Plus
  |  Minus
  |  SemiColon
  |  EscapeSequence
  |  UnderScore
  |  Numbers
  |  '.'
  |  '/'
  |  '.'
  |  '#'
  |  '['
  |  ']'
  |  '('
  |  ')'
  |  Colon
  |  Equal
  |  '?'
  |  '<'
  |  '>'
  |  '&'
  |  '"'
  |  Any
  ;

text_fragment
  :  text_fragment_start
  |  Block
  |  Bullet
  |  Literal
  |  Enumerated
  |  Comment
  |  Space
  ;

starText
  :  Star+ starAtoms Star+
  |  Star Star
  |  Star Space+ starAtoms
  ;

starAtoms
  :  starAtom+ (Star* starAtom)*
  ;
  
starAtom
  :  ~(Star | LineBreak)
  ;

backTickText
  :  (':' titled=backTickAtoms ':')? body
  ;

body
  :  (BackTick backTickText BackTick)
  |  (BackTick backTickAtoms BackTick+)
  ;

backTickAtoms
  :  backTickAtom+
  ;

backTickAtom
  :  ~BackTick
  |  BackTick ~BackTick
  ;

reference
  :  Any+ UnderScore
  ;

referenceIn
  :  UnderScore hyperlinkAtom+ ':' Space url
  ;

hyperlinkTarget
  :  UnderScore Any+
  ;
  
hyperlink
  :  BackTick hyperlinkAtom+ Space '<' url '>' BackTick UnderScore
  ;
 
hyperlinkDoc
  :  ':doc:' BackTick hyperlinkAtom+ Space '<' url '>' BackTick
  |  ':doc:' BackTick url BackTick
  ;

url
  :  urlAtom+
  ;
  
urlAtom
  :  ~( LineBreak | BackTick )
  ;
  
hyperlinkAtom
  :  ~( LineBreak | '<' | '>' | '`' | '*' )
  ;

listItemBullet
  :  Space* Bullet (paragraph+)?
  ;

listItemEnumerated
  :  Enumerated paragraph+ ending=LineBreak? 
  ;
  
Literal
  :  Colon LineBreak LineBreak* Colon Colon
  ;

Section
  :  ('-' | '=' | '+') ('-' | '=' | '+') ('-' | '=' | '+')+
  ;
  
Bullet
  :  Plus Space 
  |  Minus Space
  ;
  
Enumerated
  :  (Numbers '.' Space)
  ;

 
Numbers
  : [0-9]+
  ;
  
SemiColon
  :  ';'
  ;
  
Colon
  :  ':'
  ;

Equal
  :  '='
  ;

Plus
  :  '+'
  ;

Minus
  :  '-'
  ;

Block
  :  '| '
  ;

Comment
  :  ('.. ' LineBreak?)
  |  ('..' LineBreak)
  ;

UnderScore
  :  '_'
  ;

BackTick
  :  '`'
  ;

Star
  :  '*'
  ;

Space
  :  ' ' 
  |  '\t'
  ;

EscapeSequence
  :  '\\' ('\\' | '*')
  ;

LineBreak
  :  '\r'? '\n'
  ;

Any
  :  .
  ;